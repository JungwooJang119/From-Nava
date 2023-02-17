using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorUtils : MonoBehaviour
{
    private static List<Vector2> vectors = new List<Vector2>{Vector2.right, Vector2.up, Vector2.left, Vector2.down};

    public static Vector2 DirToVector(int dir) {
        return vectors[dir];
    }

    public static int VectorToDir(Vector2 vector) {
        return vectors.IndexOf(vector);
    }
}
