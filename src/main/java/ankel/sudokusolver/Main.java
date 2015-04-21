package ankel.sudokusolver;

import java.io.IOException;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.Set;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.atomic.AtomicLong;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.common.base.Charsets;
import com.google.common.base.Joiner;
import com.google.common.base.Stopwatch;
import com.google.common.collect.ImmutableList;
import com.google.common.collect.Sets;
import com.google.common.io.Resources;

import ankel.sudokusolver.constraint.Checker;
import ankel.sudokusolver.constraint.SudokuChecker;

/**
 * @author Binh Tran
 */
public class Main
{

  private static final AtomicLong tries = new AtomicLong();
  private static final Set<Integer> ONE_THROUGH_NINE;
  private static final ObjectMapper mapper = new ObjectMapper();
  static {
    ONE_THROUGH_NINE = Sets.newConcurrentHashSet();
    for (int i = 1; i <=9; ++i)
    {
      ONE_THROUGH_NINE.add(i);
    }
  }

  public static void main(String[] args) throws Exception
  {
    Stopwatch watch = Stopwatch.createStarted();
    List<Checker> constraints = new ArrayList<>();
    constraints.add(new SudokuChecker(getGridListFromResource("col.txt")));
    constraints.add(new SudokuChecker(getGridListFromResource("grid.txt")));
    constraints.add(new SudokuChecker(getGridListFromResource("row.txt")));

    final List<Integer> puzzle = getGridListFromResource("puzzle.txt");
    final List<Set<Integer>> validNumberPerCell = new ArrayList<>();
    for (int i = 0; i < 81; ++i)
    {
      validNumberPerCell.add(null);
    }

    final int start = findStart(constraints, puzzle, validNumberPerCell);
    System.out.println(start);

    solve(constraints, puzzle, validNumberPerCell, start, start);

    printPuzzle(puzzle);
    System.out.println(tries.get());
    watch.stop();
    System.out.println(watch.elapsed(TimeUnit.MILLISECONDS) + "ms");
  }

  private static int findStart(final List<Checker> constraints, final List<Integer> puzzle,
      final List<Set<Integer>> validNumberPerCell)
  {
    int highestOrder = 0,
        start = 0;
    for (int i = 0; i < puzzle.size(); ++i)
    {
      if (puzzle.get(i) != 0)
      {
        continue;
      }
      final int pos = i;
      final Set<Integer> invalidNumbers = constraints.parallelStream()
          .map((c) -> c.order(puzzle, pos))
          .reduce(Sets::union)
          .get();
      validNumberPerCell.set(pos, Sets.difference(ONE_THROUGH_NINE, invalidNumbers));

      final int order = invalidNumbers.size();
      if (order > highestOrder)
      {
        highestOrder = order;
        start = i;
      }
    }

    return start;
  }

  private static void printPuzzle(final List<Integer> puzzle)
  {
    for (int i = 0; i < puzzle.size(); ++i)
    {
      System.out.print(puzzle.get(i) + " ");
      if (i % 9 == 8)
      {
        System.out.println();
      }
    }
  }

  private static boolean solve(final List<Checker> constraints, final List<Integer> puzzle,
      final List<Set<Integer>> validNumberPerCell, int index, final int start)
  {
    if (index == start && puzzle.get(start) != 0)
    {
      return true;
    }

    final int size = puzzle.size();

    while (puzzle.get(index) != 0)
    {
      index = (index + 1) % size;
      if (index == start)
      {
        return true;
      }
    }

    final int nextPos = index;

    for (Integer i : validNumberPerCell.get(nextPos))
    {
      final int nextValue = i;
      tries.incrementAndGet();
      final boolean validMove = constraints.parallelStream()
          .map((c) -> c.check(puzzle, nextValue, nextPos))
          .reduce((b1, b2) -> b1 && b2)
          .get();

      if (validMove)
      {
        puzzle.set(index, i);
        if (solve(constraints, puzzle, validNumberPerCell, (index + 1) % size, start))
        {
          return true;
        }
      }
    }
    puzzle.set(index, 0);
    return false;
  }

  private static List<Integer> getGridListFromResource(final String fileName) throws IOException
  {
    final URL resourceUrl = Resources.getResource(fileName);
    final ImmutableList<String> lines =
        Resources.asCharSource(resourceUrl, Charsets.UTF_8)
        .readLines();

    final String gridLine = Joiner.on(',').join(lines);

    List<Integer> gridList = mapper.readValue("[" + gridLine + "]",
        new TypeReference<List<Integer>>() {});

    //    System.out.println(gridList);

    if (gridList.size() != 81)
    {
      throw new IllegalStateException("Grid list is not of size 81");
    }

    return gridList;
  }
}
