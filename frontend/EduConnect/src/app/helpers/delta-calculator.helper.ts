import { diff_match_patch } from 'diff-match-patch';
import { DocumentDelta } from '../models/shared/signalr/collaboration-document-hub/document-delta';

export default class DeltaCalculator {
  private static dmp = new diff_match_patch();

  public static calculateDeltas(
    oldText: string,
    newText: string
  ): DocumentDelta[] {
    console.log('Old Text:', oldText);
    console.log('New Text:', newText);
    const diffs = this.dmp.diff_main(oldText, newText);
    this.dmp.diff_cleanupSemantic(diffs);

    let currentPos = 0;
    const deltas: DocumentDelta[] = [];

    for (const [operation, text] of diffs) {
      switch (operation) {
        case diff_match_patch.DIFF_DELETE:
          deltas.push({
            position: currentPos,
            insert: null,
            delete: text.length,
          });
          break;

        case diff_match_patch.DIFF_INSERT:
          deltas.push({
            position: currentPos,
            insert: text,
            delete: null,
          });
          currentPos += text.length;
          break;

        case diff_match_patch.DIFF_EQUAL:
          currentPos += text.length;
          break;
      }
    }

    const result = this.mergeAdjacentDeltas(deltas);
    console.log('Deltas after merge:', result);
    return result;
  }

  private static mergeAdjacentDeltas(deltas: DocumentDelta[]): DocumentDelta[] {
    return deltas.reduce((merged, delta) => {
      const last = merged[merged.length - 1];

      if (last && last.position + (last.delete || 0) === delta.position) {
        // Merge delete operations
        if (last.delete && delta.delete) {
          last.delete += delta.delete;
          return merged;
        }

        // Merge insert operations
        if (last.insert && delta.insert && last.position === delta.position) {
          last.insert += delta.insert;
          return merged;
        }
      }

      merged.push(delta);
      return merged;
    }, [] as DocumentDelta[]);
  }
}
