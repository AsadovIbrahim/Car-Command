using Server;
using System.Data;
using System.Text.Json;

var cars = ReadData< List<Car>> ("MOCK_DATA");


T? ReadData<T>(string filename) where T : new()
{
    T? readData = new T();

    JsonSerializerOptions op = new JsonSerializerOptions();
    op.WriteIndented = true;
    using FileStream fs = new FileStream(filename + ".json", FileMode.OpenOrCreate);
    if (fs.Length != 0) readData = JsonSerializer.Deserialize<T>(fs, op);

    return readData;
}

void WriteData<T>(T? list, string filename)
{
    JsonSerializerOptions op = new();
    op.WriteIndented = true;

    File.WriteAllText(filename + ".json", JsonSerializer.Serialize(list, op));
}

Car GetById(int id)
{
    foreach(var car in cars)
    {
        if(car.Id==id) return car;
    }
    return null;
}

bool Add(Car car)
{
    cars.Add(car);
    return true;
}

bool Update(Car car)
{
    var index=cars.FindIndex(c=>c.Id==car.Id);
    if (index != -1)
    {
        cars[index] = car;
        return true;
    }
    else return false;
}

bool Delete(int id)
{
    Car car = GetById(id);
    if (car is null) return false;
    cars.Remove(car);
    return true;
}