﻿using Bookmark_Manager_Client.Model;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static bool DeleteCategoryWithChildCategories(this Collection<Category> categories, Category category)
        {
            foreach(var currentCategory in categories)
            {
                if(currentCategory.ID == category.ID)
                {
                    categories.Remove(currentCategory);
                    return true;
                }
                else
                {
                    if (deleteCategoryRecurse(currentCategory, category))
                        return true;
                }
            }
            return false;
        }
        private static bool deleteCategoryRecurse(Category currentCategory, Category category) 
        {
            foreach(var childCategory in currentCategory.ChildCategories)
            {
                if(childCategory.ID == category.ID)
                {
                    currentCategory.ChildCategories.Remove(childCategory);
                    return true;
                }
                else
                    if(deleteCategoryRecurse(childCategory, category))
                        return true;
            }
            return false;
        }

        public static bool ReplaceCategory(this Collection<Category> categories, Category categoryToReplace, Category categoryReplaceble)
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
        public static bool ReplaceCategoryWithId(this Collection<Category> categories, uint categoryToReplace, Category categoryReplaceble)
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

