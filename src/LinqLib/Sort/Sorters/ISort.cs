
namespace LinqLib.Sort.Sorters
{
  internal interface ISort<TKey>
  {
    MapItem<TKey>[] Sort();
  }
}
