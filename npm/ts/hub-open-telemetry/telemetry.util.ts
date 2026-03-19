const IOS_DEVICE_MAPPING: Map<string, string> = new Map([
    ["320x480", "iPhone 4S, 4, 3GS, 3G, 1st gen"],
    ["320x568", "iPhone 5, SE 1st Gen, 5C, 5S"],
    ["375x667", "iPhone SE 2nd Gen, 6, 6S, 7, 8"],
    ["375x812", "iPhone X, XS, 11 Pro, 12 Mini, 13 Mini"],
    ["390x844", "iPhone 13, 13 Pro, 12, 12 Pro"],
    ["414x736", "iPhone 8+"],
    ["414x896", "iPhone 11, XR, XS Max, 11 Pro Max"],
    ["428x926", "iPhone 13 Pro Max, 12 Pro Max"],
    ["476x847", "iPhone 7+, 6+, 6S+"],
    ["744x1133", "iPad Mini 6th Gen"],
    [
        "768x1024",
        "iPad Mini (5th Gen), iPad (1-6th Gen), iPad Pro (1st Gen 9.7), iPad Mini (1-4), iPad Air(1-2)",
    ],
    ["810x1080", "iPad 7-9th Gen"],
    ["820x1180", "iPad Air (4th gen)"],
    ["834x1194", "iPad Pro (3-5th Gen 11)"],
    ["834x1112", "iPad Air (3rd gen), iPad Pro (2nd gen 10.5)"],
    ["1024x1366", "iPad Pro (1-5th Gen 12.9)"],
]);

const DESKTOP_DEVICE_MAPPING: Map<string, string> = new Map([
    ["Win32", "Windows"],
    ["Linux", "Linux"],
    ["MacIntel", "Mac OS"],
]);


const getAndroidDeviceName = (): string => {
    const ua = window.navigator.userAgent;
    const androidIndex = ua.indexOf("Android");

    if (androidIndex === -1) return "Android";

    const androidSection = ua.slice(androidIndex);
    const deviceName = androidSection.slice(
        androidSection.indexOf("; ") + 2,
        androidSection.indexOf(")")
    );

    return deviceName ? deviceName.trim().split(" ")[0] : "Android";
};

const getIosDeviceName = (): string => {
    // Use screen.width/height to look up mapping
    const screenResolution: string = `${window.screen.width}x${window.screen.height}`;
    return IOS_DEVICE_MAPPING.get(screenResolution) ?? "iPhone/iPad";
};

const getDesktopDeviceName = (): string => {
    // Extending Navigator for experimental userAgentData support in TS
    const nav = window.navigator as any;
    const platform = nav?.userAgentData?.platform || nav?.platform || "unknown";

    return DESKTOP_DEVICE_MAPPING.get(platform) ?? "Desktop";
};

const getDeviceName = (): string => {
    const ua = window.navigator.userAgent.toLowerCase();
    const isMobileDevice = ua.includes("mobi");

    if (isMobileDevice) {
        if (ua.includes("android")) {
            return getAndroidDeviceName();
        }
        return getIosDeviceName();
    }

    return getDesktopDeviceName();
};

const getDevicePosture = () => {
    interface NavigatorPosture extends Navigator {
        devicePosture?: {
            type: 'continuous' | 'folded';
            readonly values: string[];
            onchange: ((this: Screen, ev: Event) => any) | null;
        };
    }

    const nav = navigator as NavigatorPosture;
    return nav.devicePosture?.type || 'not-supported';
};

const getDeviceMemory = () => {
    interface NavigatorWithMemory extends Navigator {
        readonly deviceMemory?: number;
    }

    const nav = navigator as NavigatorWithMemory;
    return nav.deviceMemory || 'unknown';
};

const getOnlineStatus = () => {
    return navigator.onLine;
};

const getPreferredLanguages = () => {
    return (navigator.languages || [navigator.language]).join(',');
};

const getStorageData = async () => {
    if (navigator.storage && navigator.storage.estimate) {
        const { usage = 0, quota = 0 } = await navigator.storage.estimate();
        const data = {
            usageMB: (usage / (1024 * 1024)).toFixed(2),
            quotaMB: (quota / (1024 * 1024)).toFixed(2)
        };
        return JSON.stringify(data);
    }
    return '';
};

export { getDeviceName, getDevicePosture, getDeviceMemory, getOnlineStatus, getPreferredLanguages, getStorageData };