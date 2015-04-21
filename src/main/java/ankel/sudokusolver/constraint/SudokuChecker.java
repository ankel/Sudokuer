package ankel.sudokusolver.constraint;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import com.google.common.base.Preconditions;

/**
 * @author Binh Tran
 */
public class SudokuChecker implements Checker
{
  private final List<Integer> gridList;
  private final List<List<Integer>> gridMap;

  public SudokuChecker (final List<Integer> gridList)
  {
    Preconditions.checkState(gridList.size() == 81, "Grid list is not of size 81");

    this.gridList = gridList;
    gridMap = new ArrayList<>();

    for (int i = 0; i < 10; ++i)
    {
      gridMap.add(new ArrayList<>());
    }

    for (int i = 0; i < 81; ++i )
    {
      gridMap.get(gridList.get(i)).add(i);
    }
  }

  @Override
  public boolean check(final List<Integer> sofar, final int nextValue, final int nextPosition)
  {
    int currentType = gridList.get(nextPosition);
    for (int pos : gridMap.get(currentType))
    {
      if (sofar.get(pos).equals(nextValue))
      {
        return false;
      }
    }
    return true;
  }

  @Override
  public Set<Integer> order(final List<Integer> puzzle, final int nextPosition)
  {
    int currentType = gridList.get(nextPosition);
    Set<Integer> ret = new HashSet<>();
    for (int pos : gridMap.get(currentType))
    {
      ret.add(puzzle.get(pos));
    }

    return ret;
  }
}
