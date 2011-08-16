//Note: this file have its Build Action property set to AndroidJavaSource in the Solution Explorer
package com.monostockportfolio;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.view.WindowManager;
import android.view.MotionEvent;

public class SplashActivity extends Activity {

	protected int _splashTime = 3000;
	Handler handler;
	SplashScreenRunner runner;

	@Override
	public void onCreate(Bundle icicle) {
		super.onCreate(icicle);
		
		//Ensure we use the full screen area
		//Note that specifying the Theme.Translucent.NoTitleBar.Fullscreen (or even Theme.Wallpaper.NoTitleBar.Fullscreen) style
		//for the activity in the Android manifest is easier than these two calls
        //requestWindowFeature(android.view.Window.FEATURE_NO_TITLE);
		//getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
		
		// Have the system blur any windows behind this one.
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_BLUR_BEHIND, WindowManager.LayoutParams.FLAG_BLUR_BEHIND);
        //Inflate the UI
		setContentView(R.layout.splashscreen);
	}

	@Override
	public void onStart() {
		super.onStart();
		handler = new Handler();
		runner = new SplashScreenRunner();
		handler.postDelayed(runner, _splashTime);		
	}

	public boolean onTouchEvent(MotionEvent event) {
		//Exit splash screen on touch
		if (event.getAction() == MotionEvent.ACTION_UP)
		{
			//Cancel the delayed invocation of main screen and instead do it immediately
			handler.removeCallbacks(runner);
			handler.post(runner);
		}
		return true;
	}
	
	class SplashScreenRunner implements Runnable {
		public void run() {
			Intent i = new Intent();
			//Params: package name (as per AndroindManifest.xml) and lower-case-namespace-qualified
			//activity class (or name as per the obj\Debug\android\AndroidManifest.xml)
			i.setClassName("com.monostockportfolio", "monostockportfolio.activites.mainscreen.MainActivity");
			startActivity(i);
			SplashActivity.this.finish();
		}
	}
}