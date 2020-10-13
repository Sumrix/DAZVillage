using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    [ExecuteInEditMode]
    public class RelativeSizeTable :
        MonoBehaviour
    {
        [SerializeField]
        private RelativeSizeType _sizeType;
        [SerializeField]
        private float _count = 1;
        [SerializeField]
        private float _ratioOfWidthToHeight = 1;

        void Start()
        {
            ChangeSize();
        }
        void OnEnable()
        {
            ChangeSize();
        }
        void OnRectTransformDimensionsChange()
        {
            ChangeSize();
        }
        void OnValidate()
        {
            ChangeSize();
        }
        void ChangeSize()
        {
            // Проверка чтобы не делить на 0
            if (!Mathf.Approximately(_count, 0) && !Mathf.Approximately(_ratioOfWidthToHeight, 0))
            {
                var rect = GetComponent<RectTransform>();
                var grid = GetComponent<GridLayoutGroup>();
                float width, height;
                // Выбираем по какому размеру надо подстроиться
                if (_sizeType == RelativeSizeType.Width)
                {
                    // Вычисляем сперва ширину и под него подстраиваем высоту
                    // Нужно так же учесть расстояние между ячейками
                    width = rect.rect.width / _count - grid.spacing.x * (1 - 1 / _count);
                    height = width / _ratioOfWidthToHeight;
                }
                else
                {
                    height = rect.rect.height / _count - grid.spacing.y * (1 - 1 / _count);
                    width = height * _ratioOfWidthToHeight;
                }
                grid.cellSize = new Vector2(width, height);
            }
        }
    }

    public enum RelativeSizeType
    {
        Width,
        Height
    }
}