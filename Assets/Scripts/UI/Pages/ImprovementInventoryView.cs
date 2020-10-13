namespace GameUI
{
    public class ImprovementInventoryView :
        PageView
    {
        protected override void OnEnable()
        {
            base.OnEnable();

            var inventory = (InventoryPage)CurrentPage;
            // Деактивировать ненужные предметы
            foreach (var collectionView in inventory.CollectionViews)
            {
                bool equipmentSelected = false;
                for (int i = 0; i < collectionView.DataSource.Count; i++)
                {
                    var viewItem = (ItemView)collectionView.ViewItems[i];
                    var item = collectionView.DataSource[i];
                    if (item != null)
                    {
                        if (item is Equipment)
                        {
                            if (!equipmentSelected)
                            {
                                collectionView.Select(i);
                                equipmentSelected = true;
                            }
                        }
                        else
                        {
                            viewItem.SetActive(false);
                            viewItem.GetComponent<Clickable>().IsActive = false;
                        }
                        viewItem.GetComponent<Draggable>().IsActive = false;
                    }
                }
            }
        }
        protected override void OnDisable()
        {
            base.OnDisable();

            var inventory = (InventoryPage)CurrentPage;
            // Сделать активными деактевированные предметы
            foreach (var collectionView in inventory.CollectionViews)
            {
                for (int i = 0; i < collectionView.DataSource.Count; i++)
                {
                    var viewItem = (ItemView)collectionView.ViewItems[i];
                    var item = collectionView.DataSource[i];
                    if (item != null)
                    {
                        viewItem.SetActive(true);
                        viewItem.GetComponent<Clickable>().IsActive = true;
                        viewItem.GetComponent<Draggable>().IsActive = true;
                    }
                }
            }
        }
    }
}