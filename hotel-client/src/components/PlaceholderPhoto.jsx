// Generated illustration used in place of a real photo everywhere a hotel/room
// image would appear. Deterministic per (seed, index) so the same room/hotel
// always shows the same art, and different rooms look visually distinct.
// Guarantees no people ever appear in any "photo" on the site.

const GRADIENTS = [
  ["#6e1e3a", "#10233f"],
  ["#10233f", "#1b3a63"],
  ["#8c2c4c", "#4e1329"],
  ["#1b3a63", "#0a1626"],
  ["#4e1329", "#10233f"],
  ["#10233f", "#6e1e3a"],
];

// Object/scenery glyphs only - no human figures.
const ICON_SETS = {
  hotel: ["🏨", "🌆", "🛎"], // 🏨 🌆 🛎️
  standard: ["🛏", "🚪", "🗝"], // 🛏 🚪 🗝
  deluxe: ["🛏", "👑", "🌅"], // 🛏 👑 🌅
  family: ["🛏", "🛋", "🌴"], // 🛏 🛋 🌴
  suite: ["👑", "🌅", "🛋"], // 👑 🌅 🛋
  room: ["🛏", "🚪", "🗝"],
};

function hashSeed(value) {
  const str = String(value);
  let hash = 0;
  for (let i = 0; i < str.length; i++) {
    hash = (hash * 31 + str.charCodeAt(i)) >>> 0;
  }
  return hash;
}

export default function PlaceholderPhoto({ seed, index = 0, category = "hotel", className = "" }) {
  const hash = hashSeed(`${seed}-${index}`);
  const [from, to] = GRADIENTS[hash % GRADIENTS.length];
  const icons = ICON_SETS[category] ?? ICON_SETS.hotel;
  const icon = icons[index % icons.length];
  const gradientId = `grad-${hash}`;

  return (
    <svg
      viewBox="0 0 400 300"
      className={className}
      role="img"
      aria-label={`${category} illustration`}
      preserveAspectRatio="xMidYMid slice"
    >
      <defs>
        <linearGradient id={gradientId} x1="0%" y1="0%" x2="100%" y2="100%">
          <stop offset="0%" stopColor={from} />
          <stop offset="100%" stopColor={to} />
        </linearGradient>
      </defs>
      <rect width="400" height="300" fill={`url(#${gradientId})`} />
      <circle cx={70 + (hash % 60)} cy={60 + (hash % 40)} r="90" fill="rgba(255,255,255,0.05)" />
      <circle cx={340 - (hash % 50)} cy={240 - (hash % 30)} r="70" fill="rgba(255,255,255,0.06)" />
      <text x="200" y="175" fontSize="72" textAnchor="middle" dominantBaseline="middle">
        {icon}
      </text>
    </svg>
  );
}
