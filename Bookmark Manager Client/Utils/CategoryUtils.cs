using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.Utils
{
    public static class CategoryUtils
    {
        public static Category FindCategory(this ObservableCollection<Category> categories, Category category)
        {
            foreach (var cat in categories)
            {
                if (cat.ID == category.ID) return cat;
                var item = FindCategoryInChild(category, cat);
                if (item != null) return item;
            }
            return null;
        }
        private static Category FindCategoryInChild(Category seachCategory, Category currentCategory)
        {
            foreach (var category in currentCategory.ChildCategories)
            {
                if (category.ID == seachCategory.ID) return category;
                else
                {
                    if (category.ChildCategories == null || category.ChildCategories.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return FindCategoryInChild(seachCategory, category);
                    }
                }
            }
            return null;
        }

        public static bool ReplaceCategory(this ObservableCollection<Category> categories, Category categoryToReplace, Category categoryReplaceble)
        {
            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i] == categoryToReplace)
                {
                    categories[i] = categoryReplaceble;
                }
                
                if (ReplaceCategoryInChild(categories[i], categoryToReplace, categoryReplaceble))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool ReplaceCategoryInChild(Category currentCategory, Category categoryToReplace, Category categoryReplaceble)
        {
            for(int i = 0; i < currentCategory.ChildCategories.Count; i++)
            {
                if (currentCategory.ChildCategories[i] == categoryToReplace)
                {
                    currentCategory.ChildCategories[i] = categoryReplaceble;
                    return true;
                }
                else
                {
                    if (ReplaceCategoryInChild(currentCategory.ChildCategories[i], categoryToReplace, categoryReplaceble))
                        return true;
                }
            }
            return false;
        }
        public static bool ReplaceCategoryWithId(this ObservableCollection<Category> categories, uint categoryToReplace, Category categoryReplaceble)
        {
            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i].ID == categoryToReplace)
                {
                    categories[i] = categoryReplaceble;
                }

                if (ReplaceCategoryInChildWithId(categories[i], categoryToReplace, categoryReplaceble))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool ReplaceCategoryInChildWithId(Category currentCategory, uint categoryToReplace, Category categoryReplaceble)
        {
            for (int i = 0; i < currentCategory.ChildCategories.Count; i++)
            {
                if (currentCategory.ChildCategories[i].ID == categoryToReplace)
                {
                    currentCategory.ChildCategories[i] = categoryReplaceble;
                    return true;
                }
                else
                {
                    if (ReplaceCategoryInChildWithId(currentCategory.ChildCategories[i], categoryToReplace, categoryReplaceble))
                        return true;
                }
            }
            return false;
        }
    }
}

