import { Injectable } from "@angular/core";
import { 
    HttpInterceptor, 
    HttpRequest, 
    HttpHandler 
} from "@angular/common/http";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private parentService: ParentService) { }
    intercept(request: HttpRequest<any>, next: HttpHandler) {
        const accessToken = this.parentService.getToken();
        request = request.clone({
            setHeaders: {
                Authorization: "Bearer " + accessToken
            }
        });
        return next.handle(request);
    }
}