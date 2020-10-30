import robotjs from 'robotjs';
import path from 'path';
import url from 'url';

export const isDevelopment = process.env.NODE_ENV !== 'production';

declare const __static: string;

/**
 * https://github.com/electron-userland/electron-webpack/issues/241#issuecomment-582920906
 */
export function getStatic(relativePath = '') {
    if (isDevelopment) {
        return url.resolve(window.location.origin, relativePath);
    }
    return path.resolve(__static, relativePath);
}

export function getOverlayPosition(windowWidth: number, windowHeight: number) {
    const padding = 5;
    const mouse = robotjs.getMousePos();
    const screen = robotjs.getScreenSize();
    const x = mouse.x + (mouse.x < screen.width / 1.25 ? padding : -windowWidth - padding);
    const y = mouse.y + (mouse.y < screen.height / 1.25 ? padding : -windowHeight - padding);

    return { x, y };
}
