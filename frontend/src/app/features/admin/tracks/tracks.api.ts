import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../../core/api/base-api.service';
import { Track, TrackDetails, TrackListItem, TrackRequest } from './track.models';

@Injectable({ providedIn: 'root' })
export class TracksApi extends BaseApiService<TrackListItem, TrackRequest, number, Track> {
  protected readonly resource = 'admin/tracks';
  protected override readonly useFormData = true;

  details(id: number): Observable<TrackDetails> {
    return this.api.get<TrackDetails>(`${this.resource}/${id}/details`);
  }
}
