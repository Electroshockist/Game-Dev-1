//speed buff
public class SpeedBuff : Buff {

    public override void Create(string name, float value) {
        Create(name, value, 0);
    }

    public override void Create(string name, float value, float duration) {
        base.Create(name, value, duration);
        type = "speed";
    }
}
