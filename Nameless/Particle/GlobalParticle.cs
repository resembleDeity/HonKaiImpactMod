namespace Nameless.Particle
{
	public abstract class GlobalParticle : NamelessType<GlobalParticle>
	{
		public override void NamelessSetup()
		{
			SetStaticDefaults();
		}

		protected override void NamelessRegister()
		{
			Instances.Add(this);
		}
	}
}
