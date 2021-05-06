namespace API
{
    public enum RequestType
    {
        Post,
        Get
    }

    public enum APICallFunction
    {
        None,
        Login,
        Register,
        AddServer,
        FetchStats,
        ServerList,
        UpdateStats,
        NetworkError,
        DeleteServer,
        UpdatePlayerCount,
    }

    public enum OrderByData
    {
        username,
        nbKills,
        nbAsteroids,
        nbDeaths,
        nbPoints,
        nbPowerUps,
        nbGames,
        nbWins,
        maxKills,
        maxPoints,
        maxPowerUps,
        maxDeaths,
    }
}
