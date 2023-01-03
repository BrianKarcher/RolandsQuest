using tk2d;

namespace tk2dRuntime2
{
	public interface ISpriteCollectionForceBuild
	{
		bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection);
		void ForceBuild();
	}
}
