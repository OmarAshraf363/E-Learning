export interface TrackListItem {
  id: number;
  name: string;
  description: string;
  cover: string | null;
  coursesCount: number;
}

export interface Track {
  id: number;
  name: string;
  description: string;
  cover: string | null;
}

export interface TrackDetails extends Track {
  courses: { id: number; name: string; cover: string | null; instructorName: string | null; price: number | null; description: string }[];
}

export interface TrackRequest {
  name: string;
  description: string;
  cover: File | null;
}
