﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenNLP.Tools.Util.Trees.TRegex.Tsurgeon
{
    public class CoindexationGenerator
    {
        /**
   * We require at least one character before the - so that negative
   * numbers do not get treated as indexed nodes.  This seems more
   * likely than a node having an index on an otherwise blank label.
   */
  private static readonly Regex coindexationPattern = new Regex(".+?-([0-9]+)$", RegexOptions.Compiled);

  private int lastIndex;

  public void setLastIndex(Tree t) {
    lastIndex = 0;
      var iterator = t.iterator();
      while(iterator.hasNext())
      {
          var node = iterator.next();
    /*foreach (Tree node in t) {*/
      String value = node.label().value();
      if (value != null) {
        var m = coindexationPattern.Match(value);
        if (m.Success) {
          int thisIndex = 0;
          try {
            thisIndex = int.Parse(m.Groups[1].Value);
          } catch (FormatException e) {
            // Ignore this exception.  This kind of exception can
            // happen if there are nodes that happen to have the
            // indexing character attached, even despite the attempt
            // to ignore those nodes in the pattern above.
          }
          lastIndex = Math.Max(thisIndex, lastIndex);
        }
      }
    }
  }

  public int generateIndex() {
    lastIndex = lastIndex+1;
    return lastIndex;
  }
    }
}