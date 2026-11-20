import { Pipe, PipeTransform } from '@angular/core';
import { environment } from '../../../environments/environment';

/**
 * Turns a file path returned by the API ("/Covers/x.png", "videos/y.mp4") into a full URL.
 * Absolute URLs (http...) are returned unchanged.
 *   <img [src]="course.imgCover | fileUrl" />
 */
@Pipe({ name: 'fileUrl' })
export class FileUrlPipe implements PipeTransform {
  transform(path: string | null | undefined, fallback = ''): string {
    if (!path) return fallback;
    if (/^(https?:)?\/\//i.test(path) || path.startsWith('data:')) return path;
    return `${environment.fileBaseUrl}/${path.replace(/^\/+/, '')}`;
  }
}
