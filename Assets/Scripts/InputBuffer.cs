using System.Collections.Generic;
using UnityEngine;

public class InputBuffer
{
    private Queue<float> inputTimestamps = new Queue<float>();
    private float bufferTime;

    public InputBuffer(float bufferTime)
    {
        this.bufferTime = bufferTime;
    }

    public void Register()
    {
        CleanExpiredInputs();

        if (inputTimestamps.Count == 0)
            inputTimestamps.Enqueue(Time.time);
    }

    public bool Peek()
    {
        CleanExpiredInputs();
        return inputTimestamps.Count > 0;
    }

    public void Consume()
    {
        CleanExpiredInputs();

        if (inputTimestamps.Count > 0)
        {
            inputTimestamps.Dequeue();
        }
    }

    private void CleanExpiredInputs()
    {
        while (inputTimestamps.Count > 0)
        {
            float inputTime = inputTimestamps.Peek();
            if (Time.time - inputTime > bufferTime)
                inputTimestamps.Dequeue();
            else
                break;
        }
    }
}
