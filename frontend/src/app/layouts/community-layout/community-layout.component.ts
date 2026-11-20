import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

/** Community feed layout (legacy _HomeLayout: no navbar, grey background). */
@Component({
  selector: 'app-community-layout',
  imports: [RouterOutlet],
  template: `
    <div class="layout-community community-page">
      <router-outlet />
    </div>
  `,
  styles: `
    .community-page { background: #f3f2f0; min-height: 100vh; }
  `,
})
export class CommunityLayoutComponent {}
