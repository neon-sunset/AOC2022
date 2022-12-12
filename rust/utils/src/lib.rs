use std::{
    fs::File,
    io::{Error, Read},
};

pub fn get_input() -> Result<String, Error> {
    let mut file = File::open("Input")?;
    let mut buf = String::with_capacity(file.metadata()?.len() as usize);

    file.read_to_string(&mut buf)?;

    Ok(buf)
}
