using System.Net.Http.Json;
using System.Text.Json;

Maze maze = new Maze();
ObjMaze objMaze = new ObjMaze()
{
    Width = 3,
    Height = 3,
    Url = "https://mazerunnerapi6.azurewebsites.net/api/Maze?code=CTLH2JGw02ntEMlwXANzIegaNFGi/vSE34NSvgar5WYFb1x349z8jw=="
};

//Paso 1:
var newMaze = await maze.CreateNewRandomMaze(objMaze);
Console.WriteLine(newMaze);

//Paso 2;
ObjGame objGame = new ObjGame()
{
    Operation = "Start",
    Url = $"https://mazerunnerapi6.azurewebsites.net/api/Game/{newMaze.MazeUid}?code=CTLH2JGw02ntEMlwXANzIegaNFGi/vSE34NSvgar5WYFb1x349z8jw=="
};
var game = await maze.CreateGameWithNewMaze(objGame);
Console.WriteLine(game);

//Paso 3
//ObjCurrentPosition objCurrentPosition = new ObjCurrentPosition()
//{
//    Url = $"https://mazerunnerapi6.azurewebsites.net/api/Game/{game.MazeUid}/{game.GameUid}?code=CTLH2JGw02ntEMlwXANzIegaNFGi/vSE34NSvgar5WYFb1x349z8jw=="
//};
//var currentPosition = await maze.GameCurrentPosition(objCurrentPosition);
//Console.WriteLine(currentPosition);

//Intermedio
//var url = $"https://mazerunnerapi6.azurewebsites.net/api/Maze/{game.MazeUid}?code=CTLH2JGw02ntEMlwXANzIegaNFGi/vSE34NSvgar5WYFb1x349z8jw==";
//var debugging = await maze.DebugingPurpouses(url);
//Console.WriteLine(debugging);

//Paso 4
char[,] laberinto = new char[objMaze.Height, objMaze.Width];
Organizar();
int i = 0;
List<ObjGameCurrentPositionMazeBlockView> lista = new List<ObjGameCurrentPositionMazeBlockView>();
await buscarSalida("");
Console.WriteLine();
Console.WriteLine("Felicitaciones, llegastes al final");
async Task<bool> buscarSalida(string latitude, int f = 0, int c = 0)
{
    var currentPosition = new ObjGameCurrentPositionAnswer();
    objGame = new ObjGame()
    {
        Operation = latitude,
        Url = $"https://mazerunnerapi6.azurewebsites.net/api/Game/{game.MazeUid}/{game.GameUid}?code=CTLH2JGw02ntEMlwXANzIegaNFGi/vSE34NSvgar5WYFb1x349z8jw=="
    };

    if (!string.IsNullOrEmpty(latitude))
    {
        currentPosition = await maze.MoveLatitude(objGame);
        if (currentPosition.MazeBlockView != null)
        {
            currentPosition.MazeBlockView.Latitude = latitude;
            switch (latitude)
            {
                case "GoEast":
                    currentPosition.MazeBlockView.ReverseLatitude = "GoWest";
                    break;
                case "GoNorth":
                    currentPosition.MazeBlockView.ReverseLatitude = "GoSouth";
                    break;
                case "GoWest":
                    currentPosition.MazeBlockView.ReverseLatitude = "GoEast";
                    break;
                case "GoSouth":
                    currentPosition.MazeBlockView.ReverseLatitude = "GoNorth";
                    break;
            }
            var indexA = lista.FindIndex(x => x.CoordY == f && x.CoordX == c && x.Latitude == latitude);
            if (indexA == -1)
                lista.Add(currentPosition.MazeBlockView);
            Armar(currentPosition);
            imprimir();
        }
    }

    if ((f >= 0 && f < laberinto.GetLength(0)) && (c >= 0 && c < laberinto.GetLength(1)))
    {
        if (currentPosition.MazeBlockView != null)
        {
            if ((currentPosition.MazeBlockView.CoordX == currentPosition.MazeBlockView.CoordY) &&
            (currentPosition.MazeBlockView.CoordX == (objMaze.Height - 1) && currentPosition.MazeBlockView.CoordY == (objMaze.Width - 1)))
                return true;
        }
    }
    else
        return false;

    if ((f >= 0 && f < laberinto.GetLength(0)) && (c >= 0 && c < laberinto.GetLength(1)))
    {
        //Si llegamos a una pared o al mismo punto, no se puede resolver.
        if (laberinto[f, c] == '#' || laberinto[f, c] == '*')
        {
            if (currentPosition.MazeBlockView != null)
            {
                var indexB = lista.FindIndex(x => x.CoordY == f && x.CoordX == c && x.Latitude == latitude);
                objGame.Operation = lista[indexB].ReverseLatitude;
                currentPosition = await maze.MoveLatitude(objGame);

                //if (indexB != -1)
                //{
                //    objGame.Operation = lista[indexB - 1].Latitude;
                //    currentPosition = await maze.MoveLatitude(objGame);
                //}
            }
            return false;
        }
    }
    else
        return false;

    /*Caso intermedio
     * Se comienza a recorrer
     */
    if ((f >= 0 && f < laberinto.GetLength(0)) && (c >= 0 && c < laberinto.GetLength(1)))
    {
        laberinto[f, c] = '*'; //Linea visitada
    }
    else
        return false;

    bool camino = false;
    imprimir();

    /*Llamadas recursivas
     * //Al no encontrar la salida en los 4 movimientos, se retorna
     */
    camino = await buscarSalida("GoEast", f, (c + 1)); //Derecha
    if (camino)
        return true;

    camino = await buscarSalida("GoNorth", (f - 1), c); //Arriba
    if (camino)
        return true;

    camino = await buscarSalida("GoWest", f, (c - 1)); //Izquierda
    if (camino)
        return true;

    camino = await buscarSalida("GoSouth", (f + 1), c); //Abajo
    if (camino)
        return true;

    //Sino es el resultado, se desmarca el camino
    laberinto[f, c] = ' ';
    var indexC = lista.FindIndex(x => x.CoordY == f && x.CoordX == c);
    var item = lista[indexC];
    objGame.Operation = item.ReverseLatitude; //lista[lista.Count - 1].Latitude;
    currentPosition = await maze.MoveLatitude(objGame);
    Armar(currentPosition);
    //if(currentPosition.MazeBlockView != null)
    //    currentPosition.MazeBlockView.Name = objGame.Operation;
    ////lista.Add(currentPosition.MazeBlockView);
    //lista.RemoveAt(indexC);

    //lista.RemoveRange(indexC, (lista.Count - indexC));
    imprimir();
    return false;
}

void imprimir()
{
    Console.Clear();
    for (int i = 0; i < laberinto.GetLength(0); i++)
    {
        for (int j = 0; j < laberinto.GetLength(1); j++)
            Console.Write($"{laberinto[i, j]} ");

        Console.WriteLine();
    }
}

void Armar(ObjGameCurrentPositionAnswer objGameCurrentPositionAnswer)
{
    var f = objGameCurrentPositionAnswer.MazeBlockView.CoordY;
    var c = objGameCurrentPositionAnswer.MazeBlockView.CoordX;
    //if (laberinto[f, c] == '#')
    //    laberinto[f, c] = ' ';

    //EastBlocked
    var v1F = f;
    var v1C = c + 1;

    //NorthBlocked
    var v2F = f - 1;
    var v2C = c;

    //WestBlocked
    var v3F = f;
    var v3C = c - 1;

    //SouthBlocked
    var v4F = f + 1;
    var v4C = c;

    if (((v1F >= 0 && v1F < laberinto.GetLength(0)) && (v1C >= 0 && v1C < laberinto.GetLength(1))))
    {
        //laberinto[v1F, v1C] != '#' && 
        if (laberinto[v1F, v1C] != '*')
            laberinto[v1F, v1C] = objGameCurrentPositionAnswer.MazeBlockView.EastBlocked ? '#' : ' ';
    }

    if (((v2F >= 0 && v2F < laberinto.GetLength(0)) && (v2C >= 0 && v2C < laberinto.GetLength(1))))
    {
        //laberinto[v2F, v2C] != '#' && 
        if (laberinto[v2F, v2C] != '*')
            laberinto[v2F, v2C] = objGameCurrentPositionAnswer.MazeBlockView.NorthBlocked ? '#' : ' ';
    }

    if (((v3F >= 0 && v3F < laberinto.GetLength(0)) && (v3C >= 0 && v3C < laberinto.GetLength(1))))
    {
        //laberinto[v3F, v3C] != '#' && 
        if (laberinto[v3F, v3C] != '*')
            laberinto[v3F, v3C] = objGameCurrentPositionAnswer.MazeBlockView.WestBlocked ? '#' : ' ';
    }

    if (((v4F >= 0 && v4F < laberinto.GetLength(0)) && (v4C >= 0 && v4C < laberinto.GetLength(1))))
    {
        //laberinto[v4F, v4C] != '#' && 
        if (laberinto[v4F, v4C] != '*')
            laberinto[v4F, v4C] = objGameCurrentPositionAnswer.MazeBlockView.SouthBlocked ? '#' : ' ';
    }
}

void Organizar()
{
    for (int i = 0; i < laberinto.GetLength(0); i++)
    {
        for (int j = 0; j < laberinto.GetLength(1); j++)
            laberinto[i, j] = ' ';
    }
}

//for (int i = 0; i < 20; i++)
//{

//    if (i > 0)
//        movimiento = "GoSouth";
//    else movimiento = "GoEast";

//    objGame = new ObjGame()
//    {
//        Operation = movimiento,
//        Url = $"https://mazerunnerapi6.azurewebsites.net/api/Game/{game.MazeUid}/{game.GameUid}?code=CTLH2JGw02ntEMlwXANzIegaNFGi/vSE34NSvgar5WYFb1x349z8jw=="
//    };
//    var latitudes = await maze.MoveLatitude(objGame);
//    Console.WriteLine(latitudes);
//}

class Maze
{
    //Paso 1
    public async Task<ObjMazeAnswer> CreateNewRandomMaze(ObjMaze objMaze)
    {
        var newMaze = new ObjMazeAnswer();
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.PostAsJsonAsync(objMaze.Url, objMaze);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                newMaze = JsonSerializer.Deserialize<ObjMazeAnswer>(body);
            }
        }
        return newMaze;
    }

    //Paso 2
    public async Task<ObjGameAnswer> CreateGameWithNewMaze(ObjGame objGame)
    {
        var gameAnswer = new ObjGameAnswer();
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.PostAsJsonAsync(objGame.Url, objGame);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                gameAnswer = JsonSerializer.Deserialize<ObjGameAnswer>(body);
            }
        }
        return gameAnswer;
    }

    //Paso 3
    public async Task<ObjGameCurrentPositionAnswer> GameCurrentPosition(ObjCurrentPosition objCurrentPosition)
    {
        var currentPositionAnswer = new ObjGameCurrentPositionAnswer();
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(objCurrentPosition.Url);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                currentPositionAnswer = JsonSerializer.Deserialize<ObjGameCurrentPositionAnswer>(body);
            }
        }
        return currentPositionAnswer;
    }

    //Intermedio
    public async Task<ObjDebugingPurpousesAnswer> DebugingPurpouses(string url)
    {
        var debuggingAnswer = new ObjDebugingPurpousesAnswer();
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                debuggingAnswer = JsonSerializer.Deserialize<ObjDebugingPurpousesAnswer>(body);
            }
        }
        return debuggingAnswer;
    }

    //Paso 4
    //because north and west are blocked, we can go to the south or the east, lest go east.
    public async Task<ObjGameCurrentPositionAnswer> MoveLatitude(ObjGame objGame)
    {
        var latitudes = new ObjGameCurrentPositionAnswer();
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.PostAsJsonAsync(objGame.Url, objGame);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                latitudes = JsonSerializer.Deserialize<ObjGameCurrentPositionAnswer>(body);
            }
        }
        return latitudes;
    }
}

class ObjMaze
{
    public int Width { get; set; }
    public int Height { get; set; }
    public string Url { get; set; } = string.Empty;
}

class ObjMazeAnswer
{
    public string MazeUid { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
}

class ObjGame {
    public string Operation { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

class ObjGameAnswer
{
    public string MazeUid { get; set; } = string.Empty;
    public string GameUid { get; set; } = string.Empty;
    public bool Completed { get; set; }
    public int CurrentPositionX { get; set; }
    public int CurrentPositionY { get; set; }
}

class ObjCurrentPosition
{
    public string Url { get; set; } = string.Empty;
}

class ObjGameCurrentPositionAnswer
{
    public ObjGameAnswer? Game { get; set; }
    public ObjGameCurrentPositionMazeBlockView? MazeBlockView { get; set; }
}

class ObjGameCurrentPositionMazeBlockView
{
    public int CoordX { get; set; }
    public int CoordY { get; set; }
    public bool NorthBlocked { get; set; }
    public bool SouthBlocked { get; set; }
    public bool WestBlocked { get; set; }
    public bool EastBlocked { get; set; }
    public string? Latitude { get; set; }
    public string? ReverseLatitude { get; set; }
}

class ObjDebugingPurpousesAnswer
{
    public string MazeUid { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public List<ObjGameCurrentPositionMazeBlockView>? Blocks { get; set; }
}