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

    /// <summary>
    /// Registers a new input in the buffer, if none are currently stored.
    /// </summary>
    public void Register()
    {
        CleanExpiredInputs();

        if (inputTimestamps.Count == 0)
            inputTimestamps.Enqueue(Time.time);
    }

    /// <summary>
    /// Returns true if there is a valid (non-expired) input in the buffer.
    /// </summary>
    public bool Peek()
    {
        CleanExpiredInputs();
        return inputTimestamps.Count > 0;
    }

    /// <summary>
    /// Consumes the oldest buffered input, if it exists.
    /// </summary>
    public void Consume()
    {
        CleanExpiredInputs();

        if (inputTimestamps.Count > 0)
        {
            inputTimestamps.Dequeue();
        }
    }

    /// <summary>
    /// Removes all inputs that are older than the buffer time limit.
    /// </summary>
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
