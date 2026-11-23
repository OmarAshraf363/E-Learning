import { CourseCard } from '../../public/data-access/catalog.models';

export interface CartItem {
  id: number;
  courseId: number;
  courseName: string;
  cover: string | null;
  instructorName: string | null;
  price: number;
  /** Always 1: a course can be in the cart only once. */
  quantity: number;
}

/** Navbar badge + the course ids the course buttons need. */
export interface CartSummary {
  count: number;
  cartCourseIds: number[];
  enrolledCourseIds: number[];
}

export interface Cart {
  items: CartItem[];
  totalCourses: number;
  subtotal: number;
  recommendations: CourseCard[];
}

export interface CheckoutResult {
  orderId: number;
  total: number;
  orderDate: string;
}
