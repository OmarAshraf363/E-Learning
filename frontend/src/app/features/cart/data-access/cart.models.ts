import { CourseCard } from '../../public/data-access/catalog.models';

export interface CartItem {
  id: number;
  courseId: number;
  courseName: string;
  cover: string | null;
  instructorName: string | null;
  /** Unit price; line total = price x quantity. */
  price: number;
  quantity: number;
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
