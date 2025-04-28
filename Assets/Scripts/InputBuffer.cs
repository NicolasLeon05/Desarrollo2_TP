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
        inputTimestamps.Enqueue(Time.time);
    }

    public bool Consume()
    {
        while (inputTimestamps.Count > 0)
        {
            float inputTime = inputTimestamps.Peek();

            if (Time.time - inputTime <= bufferTime)
            {
                inputTimestamps.Dequeue();
                return true;
            }

            inputTimestamps.Dequeue();
        }

        return false;
    }
}
