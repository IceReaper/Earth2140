using System.Linq;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Graphics;

namespace OpenRA.Mods.E2140.Graphics
{
    public class Earth2140SpriteSequenceLoader : TilesetSpecificSpriteSequenceLoader
    {
        public Earth2140SpriteSequenceLoader(ModData modData) : base(modData) { }

        public override ISpriteSequence CreateSequence(ModData modData, TileSet tileSet, SpriteCache cache, string sequence, string animation, MiniYaml info)
        {
            return new Earth2140SpriteSequence(modData, tileSet, cache, this, sequence, animation, info);
        }
    }

    public class Earth2140SpriteSequence : TilesetSpecificSpriteSequence
    {
        public Earth2140SpriteSequence(ModData modData, TileSet tileSet, SpriteCache cache, ISpriteSequenceLoader loader, string sequence, string animation, MiniYaml info)
            : base(modData, tileSet, cache, loader, sequence, animation, FlipFacings(info)) { }

        private static MiniYaml FlipFacings(MiniYaml info)
        {
            var d = info.ToDictionary();
            if (!LoadField(d, "FlipFacings", false))
                return info;

            var source = info.Value;
            info.Value = null;

            var frames = LoadField<int[]>(d, "Frames", null);
            info.Nodes.Remove(info.Nodes.First(node => node.Key == "Frames"));

            var combine = "Combine:\n";

            for (var i = 0; i < frames.Length; i++)
                combine += "\t" + source + ":\n\t\tStart:" + frames[i] + "\n\t\tAddExtension:false\n";

            for (var i = frames.Length - 2; i > 0; i--)
                combine += "\t" + source + ":\n\t\tStart:" + frames[i] + "\n\t\tAddExtension:false\n\t\tFlipX:true\n";

            info.Nodes.Add(MiniYaml.FromString(combine)[0]);

            return info;
        }
    }
}
