import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snackBar = inject(SnackbarService);

  return next(req).pipe(
    catchError((err) => {
      if (err) {
        switch (err.status) {
          case 400:
            if (err.error.errors) {
              const modelStateErrors = [];
              const errors = err.error.errors;
              for (const key in errors) {
                if (errors[key]) {
                  modelStateErrors.push(errors[key]);
                }
              }
              throw modelStateErrors.flat();
            } else {
              snackBar.error(err.error.title || err.error);
            }
            break;
          case 401:
            snackBar.error(err.error.title || err.error);
            break;
          case 404:
            router.navigateByUrl('/not-found');
            break;
          case 500:
            const navigationExtras: NavigationExtras = { state: { error: err.error } };
            router.navigateByUrl('/server-error', navigationExtras);
            break;
          default:
            snackBar.error('Something unexpected went wrong');
            console.error(err);
            break;
        }
      }
      return throwError(() => err);
    })
  );
};
