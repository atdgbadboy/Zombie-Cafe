var meta = document.createElement('meta');
meta.setAttribute('name', 'viewport');
if ( navigator.userAgent.indexOf('GT-P7510') > 0 ) {
	meta.setAttribute('content', 'width=device-width,initial-scale=2.0, minimum-scale=1.2, maximum-scale=2.0');
}else if( navigator.userAgent.indexOf('iPad') > 0 ) {
	meta.setAttribute('content', 'width=device-width,initial-scale=1.1, minimum-scale=1.1, maximum-scale=1.5');
}else{
	meta.setAttribute('content', 'width=device-width,initial-scale=1.0, minimum-scale=0.66667, maximum-scale=1.2');
}
document.getElementsByTagName('head')[0].appendChild(meta);