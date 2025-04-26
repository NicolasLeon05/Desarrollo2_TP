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
        // Limpiamos inputs expirados
        while (inputTimestamps.Count > 0)
        {
            float inputTime = inputTimestamps.Peek(); // Mira el primero sin sacarlo

            if (Time.time - inputTime <= bufferTime)
            {
                inputTimestamps.Dequeue(); // Sacamos el input porque ya lo usamos
                return true;
            }

            inputTimestamps.Dequeue(); // Si expiró, igual lo sacamos
        }

        return false;
    }
}
