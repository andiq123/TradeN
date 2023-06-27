export class ImageUtils {
  static replaceIfAbsent(photo?: string): string {
    return photo ? photo : '/assets/no-photo.jpg';
  }
}
