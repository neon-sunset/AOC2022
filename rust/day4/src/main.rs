use std::time::Instant;

fn main() -> Result<(), anyhow::Error> {
    let start = Instant::now();
    let text = utils::get_input()?;

    let (full, partial) = text.lines().fold((0, 0), |counts, line| {
        let split = line.split_once(',').unwrap();

        let (x1, x2) = parse_pair(split.0);
        let (y1, y2) = parse_pair(split.1);

        // Unlike .NET's NativeAOT, LLVM does a much better job here.
        // Unlike LLVM however, .NET's compilers do not have all the time in the universe
        // and generally assume JIT scenario - NativeAOT is a very recent addition.
        let full = x1 == y1 || (x1 < y1 && x2 >= y2) || (x1 > y1 && x2 <= y2);
        let partial = full || (x1 <= y2 && y1 <= x2);

        return match (full, partial) {
            (true, _) => (counts.0 + 1, counts.1 + 1),
            (_, true) => (counts.0, counts.1 + 1),
            _ => counts,
        };
    });

    let elapsed = start.elapsed().as_micros();
    println!("Results: {full}, {partial}. Elapsed: {elapsed}us");

    Ok(())
}

fn parse_pair(src: &str) -> (u32, u32) {
    let (start, end) = src.split_once('-').unwrap();

    (start.parse::<u32>().unwrap(), end.parse::<u32>().unwrap())
}
