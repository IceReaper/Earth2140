using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.Common.Traits.Render;
using OpenRA.Traits;

namespace OpenRA.Mods.E2140.Traits.Render
{
    public class WithMovePaletteInfo : ITraitInfo, Requires<IMoveInfo>
    {
        [Desc("Custom PlayerColorPalette: BaseName when moving")]
        [PaletteReference(true)] public readonly string Palette = null;

        public object Create(ActorInitializer init) { return new WithMovePalette(init, this); }
    }

    public class WithMovePalette : IRenderModifier
    {
        private WithMovePaletteInfo info;
        private IMove move;
        private RenderSpritesInfo renderSpritesInfo;

        public WithMovePalette(ActorInitializer init, WithMovePaletteInfo info)
        {
            this.info = info;
            move = init.Self.Trait<IMove>();
            renderSpritesInfo = init.Self.Info.TraitInfo<RenderSpritesInfo>();
        }

        public IEnumerable<IRenderable> ModifyRender(Actor self, WorldRenderer worldRenderer, IEnumerable<IRenderable> renderables)
        {
            return renderables.Select(renderable =>
            {
                if (move.IsMoving && renderable.Palette.Name == renderSpritesInfo.PlayerPalette + self.Owner.InternalName)
                    return renderable.WithPalette(worldRenderer.Palette(info.Palette + self.Owner.InternalName));

                return renderable;
            });
        }

        public IEnumerable<Rectangle> ModifyScreenBounds(Actor self, WorldRenderer wr, IEnumerable<Rectangle> bounds)
        {
            return bounds;
        }
    }
}
