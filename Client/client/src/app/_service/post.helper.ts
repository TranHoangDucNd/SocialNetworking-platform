import {CommentDto} from "../_models/PostModels";

export function countComments(comments?: CommentDto[] | undefined): number {
  if (!comments) {
    return 0;
  }
  let count = comments.length;
  comments.forEach(comment => {
    if (comment.descendants) {
      count += countComments(comment.descendants);
    }
  });
  return count;
}

export function convertToEmoji(text: string): string {
  switch (text) {
    case 'like':
      return '👍';
    case 'love':
      return '😍';
    case 'haha':
      return '😆';
    case 'wow':
      return '😲';
    case 'sad':
      return '😢';
    case 'angry':
      return '😡';
    default:
      return '👍';
  }
}
