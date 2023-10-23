using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils  {

    public static Transform FindChildRecursively(Transform parent, string nameToFind) {
        if (parent == null) {
            return null;
        }

        Transform result = parent.Find(nameToFind);

        if (result != null) {
            return result;
        }

        foreach (Transform child in parent) {
            result = FindChildRecursively(child, nameToFind);
            if (result != null) {
                return result;
            }
        }

        return null;
    }

}
