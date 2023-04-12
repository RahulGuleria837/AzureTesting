import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Display } from './display';


@Injectable({
  providedIn: 'root'
})
export class DisplayService {

  constructor(private httpClient:HttpClient) {}
  getAllData():Observable<any>
  {return this.httpClient.get<any>
  ("https://localhost:5001/getAllData");}



downloaddata():Observable<any>{
return this.httpClient.get<any>
('http://localhost:7225/api/DownloadImage/{fileName}');
  
}
downloadImage(fileName:string): Observable<Blob> {
  debugger
  return this.httpClient.get(`http://localhost:7225/api/DownloadImage/${fileName}`, { responseType: 'blob' });
}

EditdataImage(editData:Display){

  return this.httpClient.put("https://localhost:5001/updateEntityasync",editData)
}

}