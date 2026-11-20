import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

/** SweetAlert2 wrapper - same alerts the MVC app used (success toasts, delete confirmations). */
@Injectable({ providedIn: 'root' })
export class NotifyService {
  success(message: string, title = 'Done'): void {
    void Swal.fire({ icon: 'success', title, text: message, timer: 1800, showConfirmButton: false });
  }

  error(message: string, title = 'Error'): void {
    void Swal.fire({ icon: 'error', title, text: message });
  }

  /** Resolves to true when the user confirms. */
  async confirm(text = "You won't be able to revert this!", title = 'Are you sure?'): Promise<boolean> {
    const result = await Swal.fire({
      icon: 'warning',
      title,
      text,
      showCancelButton: true,
      confirmButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!',
    });
    return result.isConfirmed;
  }
}
