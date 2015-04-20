package ankel.sudokusolver.constraint;

import java.util.List;
import java.util.Set;

/**
 * Represent a constraint on the table.
 * @author Binh Tran
 */
public interface Checker
{
  /**
   * Check if the latest value fit into the puzzle so far or not.
   * @param sofar the list representing the puzzle so far
   * @param nextValue the next value to be filed into the puzzle
   * @param nextPosition the next position the value will be filled in.
   * @return true if it's a good match, false if not
   */
  boolean check(List<Integer> sofar, int nextValue, int nextPosition);

  Set<Integer> order(final List<Integer> puzzle, final int nextPosition);
}
