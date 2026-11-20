import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { CatalogApi } from '../data-access/catalog.api';

/** Legacy Customer/Home/Category without an id: list of all departments. */
@Component({
  selector: 'app-categories-page',
  imports: [RouterLink, FileUrlPipe],
  styleUrl: './category-page.component.scss',
  template: `
    <section class="departmentsList p-3 container">
      @for (dept of categories(); track dept.id) {
        <div class="oneDepart d-flex align-items-start g-4 mb-5">
          <div class="coverPart"><img [src]="dept.cover | fileUrl" [alt]="dept.name" /></div>
          <div class="discPart">
            <h5>{{ dept.name }}</h5>
            <p>{{ dept.description }}</p>
            <div><a [routerLink]="['/category', dept.id]" class="btn">Explore <i class="fa-solid fa-arrow-right"></i></a></div>
          </div>
        </div>
      }
    </section>
  `,
})
export class CategoriesPageComponent {
  readonly categories = toSignal(inject(CatalogApi).categories(), { initialValue: [] });
}
