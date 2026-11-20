import { inject, Injectable } from '@angular/core';
import { CrudStore } from '../../../core/state/crud-store';
import { Track, TrackListItem, TrackRequest } from './track.models';
import { TracksApi } from './tracks.api';

@Injectable({ providedIn: 'root' })
export class TracksStore extends CrudStore<TrackListItem, TrackRequest, number, Track> {
  constructor() {
    super(inject(TracksApi), (t) => t.id);
  }
}
