using System;

/// サーバーが返すJSONを受け取る
namespace SocialGameClient.API
{
    [Serializable]
    public class LoginRequest
    {
        public string device_id;
    }

    [Serializable]
    public class LoginResponse
    {
        public string token;
        public long user_id;
    }

    [Serializable]
    public class StateResponse
    {
        public long user_id;
        public long coin;
        public int production_rate;
        public long pending_gain;
    }

    [Serializable]
    public class ClaimResponse
    {
        public long gained;
        public long new_coin;
    }

    [Serializable]
    public class ErrorResponse
    {
        public string error;
    }
}