﻿using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.Exceptions;

namespace Terraria
{
	public partial class Recipe
	{
		#region HasX
		public bool HasResult(int itemID) => createItem.type == itemID;

		public bool HasResult(Mod mod, string itemName) {
			mod ??= Mod;

			if (!ModContent.TryFind(mod.Name, itemName, out ModItem item))
				throw new RecipeException($"The item {itemName} does not exist in the mod {mod.Name}.\r\nIf you are trying to use a vanilla item, try removing the first argument.");

			return HasResult(item.Type);
		}

		public bool HasResult(ModItem item) => HasResult(item.Type);

		public bool HasResult<T>() where T : ModItem => HasResult(ModContent.GetId<T>());

		public bool HasIngredient(int itemID) => requiredItem.Any(item => item.type == itemID);

		public bool HasIngredient(Mod mod, string itemName) {
			mod ??= Mod;

			if (!ModContent.TryFind(mod.Name, itemName, out ModItem item))
				throw new RecipeException($"The item {itemName} does not exist in the mod {mod.Name}.\r\nIf you are trying to use a vanilla item, try removing the first argument.");

			return HasIngredient(item.Type);
		}

		public bool HasIngredient(ModItem item) => HasIngredient(item.Type);

		public bool HasIngredient<T>() where T : ModItem => HasIngredient(ModContent.GetId<T>());

		public bool HasRecipeGroup(int id) => acceptedGroups.Contains(id);

		public bool HasRecipeGroup(string name) {
			if (!RecipeGroup.recipeGroupIDs.ContainsKey(name))
				throw new RecipeException($"A recipe group with the name {name} does not exist.");

			int id = RecipeGroup.recipeGroupIDs[name];
			return HasRecipeGroup(id);
		}

		public bool HasRecipeGroup(RecipeGroup group) => HasRecipeGroup(group.ID);

		public bool HasTile(int tileID) => requiredTile.Contains(tileID);

		public bool HasTile(Mod mod, string tileName) {
			mod ??= Mod;

			if (!ModContent.TryFind(mod.Name, tileName, out ModTile item))
				throw new RecipeException($"The tile {tileName} does not exist in the mod {mod.Name}.\r\nIf you are trying to use a vanilla tile, try removing the first argument.");

			return HasTile(item.Type);
		}

		public bool HasTile(ModTile tile) => HasTile(tile.Type);

		public bool HasTile<T>() where T : ModTile
			=> HasTile(ModContent.GetId<T>());

		public bool HasCondition(Condition condition) => Conditions.Contains(condition);
		#endregion

		#region TryGetX
		public bool TryGetResult(int itemID, out Item result) {
			if (createItem.type == itemID) {
				result = createItem;
				return true;
			}

			result = null;
			return false;
		}

		public bool TryGetResult(Mod mod, string itemName, out Item result) {
			mod ??= Mod;

			if (!ModContent.TryFind(mod.Name, itemName, out ModItem item))
				throw new RecipeException($"The item {itemName} does not exist in the mod {mod.Name}.\r\nIf you are trying to use a vanilla item, try removing the first argument.");

			return TryGetResult(item.Type, out result);
		}

		public bool TryGetResult(ModItem item, out Item result) => TryGetResult(item.Type, out result);

		public bool TryGetResult<T>(out Item result) where T : ModItem => TryGetResult(ModContent.GetId<T>(), out result);

		public bool TryGetIngredient(int itemID, out Item ingredient) {
			foreach (Item item in requiredItem) {
				if (item.type == itemID) {
					ingredient = item;
					return true;
				}
			}

			ingredient = null;
			return false;
		}

		public bool TryGetIngredient(Mod mod, string itemName, out Item ingredient) {
			mod ??= Mod;

			if (!ModContent.TryFind(mod.Name, itemName, out ModItem item))
				throw new RecipeException($"The item {itemName} does not exist in the mod {mod.Name}.\r\nIf you are trying to use a vanilla item, try removing the first argument.");

			return TryGetIngredient(item.Type, out ingredient);
		}

		public bool TryGetIngredient(ModItem item, out Item ingredient) => TryGetIngredient(item.Type, out ingredient);

		public bool TryGetIngredient<T>(out Item ingredient) where T : ModItem => TryGetIngredient(ModContent.GetId<T>(), out ingredient);
		#endregion

		#region RemoveX
		public bool RemoveIngredient(Item item) => requiredItem.Remove(item);

		public bool RemoveTile(int tileID) => requiredTile.Remove(tileID);

		public bool RemoveRecipeGroup(int groupID) => acceptedGroups.Remove(groupID);
		
		public bool RemoveCondition(Condition condition) => Conditions.Remove(condition);

		public bool RemoveRecipe() {
			for (int k = 0; k < numRecipes; k++) {
				if (Main.recipe[k] == this) {
					for (int j = k; j < numRecipes - 1; j++) {
						Main.recipe[j] = Main.recipe[j + 1];
					}

					Main.recipe[numRecipes - 1] = new Recipe();
					numRecipes--;
					return true;
				}
			}

			return false;
		}
		#endregion

		#region ReplaceX
		public void ReplaceResult(int itemID, int stack = 1) => createItem = new Item(itemID) { stack = stack };

		public void ReplaceResult(Mod mod, string itemName, int stack = 1) {
			mod ??= Mod;

			if (!ModContent.TryFind(mod.Name, itemName, out ModItem item))
				throw new RecipeException($"The item {itemName} does not exist in the mod {mod.Name}.\r\nIf you are trying to use a vanilla item, try removing the first argument.");

			ReplaceResult(item.Type, stack);
		}

		public void ReplaceResult(ModItem item, int stack = 1) => ReplaceResult(item.Type, stack);

		public void ReplaceResult<T>(int stack = 1) where T : ModItem => ReplaceResult(ModContent.GetId<T>(), stack);
		#endregion
	}
}