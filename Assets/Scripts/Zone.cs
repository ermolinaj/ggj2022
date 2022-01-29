internal class Zone
{
    public bool isDaytime = false;
    public int temperature = 0;
    public int populationType = 0;
    public int population = 0;



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

    public void addPopulation()
    {
        population++;
    }
}