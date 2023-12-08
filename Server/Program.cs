using Server;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
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

List<Car> ?GetAll() => cars;
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
    if (GetById(car.Id) != null) return false;
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

var server = new UdpClient(12345);
var remoteEp = new IPEndPoint(IPAddress.Parse("192.168.0.103"),27001);

while (true)
{
    var bytes = server.ReceiveAsync();
    Command command = ByteArrayToObject(bytes.Result.Buffer) as Command;
    if (command.Method == HttpMethods.GET)
    {
        var list = GetAll();
        var buffer = ObjectToByteArray(list);
        server.Send(buffer, buffer.Length,remoteEp);
    }
    else if (command.Method == HttpMethods.POST)
    {
        if (Add(command.Car))
        {
            var list = GetAll();
            var buffer = ObjectToByteArray(list);
            server.Send(buffer, buffer.Length, remoteEp);

        }

    }
    else if (command.Method == HttpMethods.PUT)
    {
        if (Update(command.Car))
        {
            var list = GetAll();
            var buffer = ObjectToByteArray(list);
            server.Send(buffer, buffer.Length, remoteEp);
        }
    }
    else if (command.Method == HttpMethods.DELETE)
    {
        if (Delete(command.Car.Id))
        {
            var list = GetAll();
            var buffer = ObjectToByteArray(list);
            server.Send(buffer, buffer.Length, remoteEp);

        }
    }
    WriteData(cars, "MOCK_DATA");
   

}

object ByteArrayToObject(byte[] byteArray)
{
    if (byteArray == null)
        return null;

    BinaryFormatter formatter = new BinaryFormatter();
    using (MemoryStream memoryStream = new MemoryStream(byteArray))
    {
        return formatter.Deserialize(memoryStream);
    }
}
byte[] ObjectToByteArray(object command)
{
    if (command == null)
        return null;

    BinaryFormatter formatter = new BinaryFormatter();
    using (MemoryStream memoryStream = new MemoryStream())
    {
        formatter.Serialize(memoryStream, command);
        return memoryStream.ToArray();
    }
}