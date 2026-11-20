import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

/** Temporary placeholder for pages that will be migrated in a later phase. */
@Component({
  selector: 'app-coming-soon',
  template: `
    <div class="text-center text-muted py-5">
      <i class="fas fa-person-digging fa-3x mb-3"></i>
      <h4>{{ title }}</h4>
      <p>This page will be migrated in {{ phase }}.</p>
    </div>
  `,
})
export class ComingSoonComponent {
  private readonly data = inject(ActivatedRoute).snapshot.data;
  readonly title = (this.data['title'] as string) ?? 'Coming soon';
  readonly phase = (this.data['phase'] as string) ?? 'a later phase';
}
