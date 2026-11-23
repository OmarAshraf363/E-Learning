import { CurrencyPipe } from '@angular/common';
import { Component, ElementRef, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { StarRatingComponent } from '../../../shared/components/star-rating.component';
import { CourseCartButtonComponent } from '../../cart/components/course-cart-button.component';
import { CartStore } from '../../cart/data-access/cart.store';
import { CourseCard } from '../data-access/catalog.models';

/**
 * Course card with the hover popover (legacy ".one-course" + "openModalWhenHover" in Home.js).
 * The popover opens on the left when the card is close to the right edge of the window.
 * The buy button is app-course-cart-button ("Add to Cart", "Go to Cart" or "Open course").
 */
@Component({
  selector: 'app-course-card',
  imports: [RouterLink, CurrencyPipe, FileUrlPipe, StarRatingComponent, CourseCartButtonComponent],
  template: `
    <div class="one-course" [routerLink]="['/courses', course().id]" (mouseenter)="open()" (mouseleave)="hovered.set(false)">
      <div class="course-video mb-3">
        <img [src]="course().image | fileUrl: 'images/online-courses.png'" [alt]="course().title" />
      </div>
      <div class="course-title"><h5>{{ course().title }}</h5></div>
      <div class="w-75 instructor"><h6>{{ course().instructorName }}</h6></div>
      <div class="rating"><app-star-rating [rate]="course().rate" /></div>
      <div class="price"><span>{{ course().price | currency }}</span></div>
      @if (cart.isEnrolled(course().id)) {
        <div class="price"><h6>You Enrolled in This Course</h6></div>
      }

      <div class="modal-test p-4" [class.modal-left]="openLeft()" [class.modal-right]="!openLeft()"
           [style.display]="hovered() ? 'block' : 'none'" (click)="$event.stopPropagation()">
        <div class="modal-course-header">
          <h3 class="modal-course-name mb-3">{{ course().title }}</h3>
          <h5>by <span>{{ course().instructorName }}</span></h5>
        </div>
        <div class="modal-course-details d-flex justify-content-between mb-2">
          <div class="course-videos"><span><i class="fas fa-play-circle me-2"></i>{{ course().videosCount }} Videos</span></div>
          <div class="course-enrollments"><span><i class="fas fa-users me-2"></i>{{ course().numOfEnrolments }} Enrolled</span></div>
        </div>
        <div class="modal-course-description mb-2"><p>{{ course().description }}</p></div>
        <div class="modal-course-about">
          <div class="course-objectives">
            <ul class="list-unstyled">
              @for (objective of course().learningObjectives; track $index) {
                <li class="d-flex justify-content-start align-items-baseline gap-1">
                  <i class="fas fa-check-circle text-success me-2"></i>
                  <p class="m-1">{{ objective }}</p>
                </li>
              }
            </ul>
          </div>
        </div>
        <div class="d-flex justify-content-center mt-2">
          <app-course-cart-button [courseId]="course().id" cssClass="btn cus-btn px-5 py-2" buttonStyle="border-radius: 5px; font-size: 1.1rem" />
        </div>
      </div>
    </div>
  `,
  styles: `
    :host { display: contents; }
    .one-course { cursor: pointer; }
    .modal-test { max-width: 700px; box-shadow: 0 8px 20px rgba(0, 0, 0, 0.1); border-radius: 10px; background-color: #fff; cursor: default; }
  `,
})
export class CourseCardComponent {
  private readonly host = inject(ElementRef<HTMLElement>);
  readonly cart = inject(CartStore);

  readonly course = input.required<CourseCard>();

  readonly hovered = signal(false);
  readonly openLeft = signal(false);

  open(): void {
    const card = (this.host.nativeElement as HTMLElement).querySelector('.one-course');
    const rect = card?.getBoundingClientRect();
    this.openLeft.set(!!rect && window.innerWidth - rect.right < 400);
    this.hovered.set(true);
  }
}
