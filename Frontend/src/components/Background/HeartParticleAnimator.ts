import { sleep } from "../../lib/sleep";
import { throttle } from "../../lib/throttle";
import theme from "../../Theme";

export interface HeartParticle {
    x: number;
    y: number;
    size: number;
    opacity: number;
    speed: number;
}

export class HeartParticleAnimator {
    private ctx: CanvasRenderingContext2D | OffscreenCanvasRenderingContext2D;
    private particles: HeartParticle[] = [];
    private animationFrameId: number | null = null;

    constructor(ctx: CanvasRenderingContext2D | OffscreenCanvasRenderingContext2D) {
        this.ctx = ctx;
        this.startAnimation();
    }

    private createHeartParticle(): HeartParticle {
        const width = this.ctx.canvas.width;
        const height = this.ctx.canvas.height;
        const max = Math.max(width, height);
        const size = Math.random() * (max / 100) + 10;
        const x = Math.random() * width;
        const y = height + size;
        const speed = Math.random() * (max / 500) + 1;
        return { x, y, size, opacity: 1, speed };
    }

    private createHeartParticleAt(x: number, y: number, opacity: number): HeartParticle {
        const width = this.ctx.canvas.width;
        const height = this.ctx.canvas.height;
        const max = Math.max(width, height);
        const size = Math.random() * (max / 100) + 10;
        const speed = Math.random() * (max / 500) + 1;
        return { x, y, size, opacity, speed };
    }

    private drawHeart(x: number, y: number, size: number, opacity: number) {
        this.ctx.save();
        this.ctx.globalAlpha = opacity;
        this.ctx.fillStyle = theme.palette.secondary.main;
        this.ctx.beginPath();
        this.ctx.moveTo(x, y);
        this.ctx.bezierCurveTo(x - size / 2, y - size / 2, x - size, y + size / 3, x, y + size);
        this.ctx.bezierCurveTo(x + size, y + size / 3, x + size / 2, y - size / 2, x, y);
        this.ctx.fill();
        this.ctx.restore();
    }

    private animate = async () => {
        this.ctx.clearRect(0, 0, this.ctx.canvas.width, this.ctx.canvas.height);
        for (let index = 0; index <= this.particles.length; index++) {
            const particle = this.particles[index]
            if (!particle) continue;

            particle.y -= particle.speed;
            particle.opacity -= 0.0025;

            if (particle.opacity <= 0 || particle.y <= -particle.size) {
                this.particles.splice(index, 1);
                index -= 1;
            } else {
                this.drawHeart(particle.x, particle.y, particle.size, particle.opacity);
            }
        }

        if (this.particles.length < Math.max(this.ctx.canvas.width, this.ctx.canvas.height) / 5) {
            this.particles.push(this.createHeartParticle());
        }

        await sleep(30)
        this.animationFrameId = requestAnimationFrame(this.animate)        
    };

    public drawNow() {
        this.ctx.clearRect(0, 0, this.ctx.canvas.width, this.ctx.canvas.height);
        this.particles.forEach(particle => this.drawHeart(particle.x, particle.y, particle.size, particle.opacity));
    }

    public startAnimation() {
        if (!this.animationFrameId) {
            this.animate();
        }
    }

    public stopAnimation() {
        if (this.animationFrameId) {
            cancelAnimationFrame(this.animationFrameId);
            this.animationFrameId = null;
        }
    }

    public addHeart(x: number, y: number) {
        return this.addHeartThrottled(x, y);
    }
    private addHeartThrottled = throttle((x, y) => this.particles.push(this.createHeartParticleAt(x, y, Math.random() * 1 + 0.1)), 100)
}
