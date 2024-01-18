import { Component } from '@angular/core';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { latLng, tileLayer } from 'leaflet';

@Component({
  selector: 'app-my-trips',
  standalone: true,
  imports: [LeafletModule],
  templateUrl: './my-trips.component.html',
  styleUrl: './my-trips.component.scss',
})
export class MyTripsComponent {
  public options = {
    layers: [
      tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 18,
        attribution: '...',
      }),
    ],
    zoom: 5,
    center: latLng(46.879966, -121.726909),
  };
}
