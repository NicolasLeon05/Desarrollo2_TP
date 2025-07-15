public interface IJumpProvider
{
    bool HasBufferedJump();
    void ConsumeBufferedJump();
    float GetJumpForce();
}
