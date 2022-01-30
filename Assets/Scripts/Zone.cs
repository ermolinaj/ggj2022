internal class Zone
{
    public bool isDaytime = false;
    public int temperature = 0;
    public int humanPopulation = 0;
    public int monsterPopulation = 0;



    public void changeTemperature(int degrees)
    {
        if (temperature + degrees <= 10 && temperature + degrees >= -10)
            temperature += degrees;
        else
        {
            if (temperature + degrees > 10)
                temperature = 10;
            else
            {
                if (temperature + degrees < -10)
                    temperature = -10;
            }
        }
    }

    public bool IsSummer()
    {
        return temperature >= 5;
    }

    public bool IsWinter()
    {
        return temperature <= -5;
    }
}